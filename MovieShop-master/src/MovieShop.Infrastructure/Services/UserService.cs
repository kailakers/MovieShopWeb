using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MovieShop.Core.Entities;
using MovieShop.Core.Exceptions;
using MovieShop.Core.Helpers;
using MovieShop.Core.Models.Request;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAsyncRepository<UserRole> _userRoleRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _encryptionService;
        private readonly IMovieService _movieService;
        private readonly IAsyncRepository<Purchase> _purchaseRepository;
        private readonly IAsyncRepository<Favorite> _favoriteRepository;
        private readonly IAsyncRepository<Review> _reviewRepository;
        private readonly IMapper _mapper;


        public UserService(ICryptoService encryptionService, IUserRepository userRepository, IMapper mapper,
            IAsyncRepository<Favorite> favoriteRepository, ICurrentUserService currentUserService, IAsyncRepository<UserRole> userRoleRepository,
            IMovieService movieService, IAsyncRepository<Purchase> purchaseRepository,
            IAsyncRepository<Review> reviewRepository)
        {
            _encryptionService = encryptionService;
            _userRepository = userRepository;
            _mapper = mapper;
            _favoriteRepository = favoriteRepository;
            _currentUserService = currentUserService;
            _userRoleRepository = userRoleRepository;
            _movieService = movieService;
            _purchaseRepository = purchaseRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<UserLoginResponseModel> ValidateUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return null;
            var hashedPassword = _encryptionService.HashPassword(password, user.Salt);
            var isSuccess =  user.HashedPassword == hashedPassword;
            var roles = await _userRoleRepository.ListAllWithIncludesAsync(ur => ur.UserId == user.Id, role => role.Role );
          
            var response = _mapper.Map<UserLoginResponseModel>(user);

            var userRoles = roles.ToList();
            if (userRoles.Any())
            {
                response.Roles = userRoles.Select(r => r.Role.Name).ToList();
            }
            return isSuccess ? response : null;
        }

        public async Task<UserRegisterResponseModel> CreateUser(UserRegisterRequestModel requestModel)
        {
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);
            if (dbUser != null &&
                string.Equals(dbUser.Email, requestModel.Email, StringComparison.CurrentCultureIgnoreCase))
                throw new ConflictException("Email Already Exits");
            var salt = _encryptionService.CreateSalt();
            var hashedPassword = _encryptionService.HashPassword(requestModel.Password, salt);
            var user = new User
            {
                Email = requestModel.Email,
                Salt = salt,
                HashedPassword = hashedPassword,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName
            };
            var createdUser = await _userRepository.AddAsync(user);

            //var response = new UserRegisterResponseModel
            //{
            //    Id = createdUser.Id, Email = createdUser.Email, FirstName = createdUser.FirstName,
            //    LastName = createdUser.LastName
            //};
            var response = _mapper.Map<UserRegisterResponseModel>(createdUser);
            return response;
        }

        public async Task<UserRegisterResponseModel> GetUserDetails(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("User", id);

            var response = _mapper.Map<UserRegisterResponseModel>(user);
            return response;
        }

        public async Task<User> GetUser(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public Task<PagedResultSet<User>> GetAllUsersByPagination(int pageSize = 20, int page = 0, string lastName = "")
        {
            throw new NotImplementedException();
        }

        public async Task AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            if (_currentUserService.UserId != favoriteRequest.UserId)
                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to purchase");
            // See if Movie is already Favorite.
            if (await FavoriteExists(favoriteRequest.UserId, favoriteRequest.MovieId))
                throw new ConflictException("Movie already Favorited");

            var favorite = _mapper.Map<Favorite>(favoriteRequest);
            await _favoriteRepository.AddAsync(favorite);
        }

        public async Task RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            var dbFavorite =
                await _favoriteRepository.ListAsync(r => r.UserId == favoriteRequest.UserId &&
                                                         r.MovieId == favoriteRequest.MovieId);
            // var favorite = _mapper.Map<Favorite>(favoriteRequest);
            await _favoriteRepository.DeleteAsync(dbFavorite.First());
        }

        public async Task<bool> FavoriteExists(int id, int movieId)
        {
            return await _favoriteRepository.GetExistsAsync(f => f.MovieId == movieId &&
                                                                 f.UserId == id);
        }

        public async Task<FavoriteResponseModel> GetAllFavoritesForUser(int id)
        {
            if (_currentUserService.UserId != id)
                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to View Favorites");

            var favoriteMovies = await _favoriteRepository.ListAllWithIncludesAsync(
                p => p.UserId == _currentUserService.UserId,
                p => p.Movie);
            return _mapper.Map<FavoriteResponseModel>(favoriteMovies);
        }

        public async Task PurchaseMovie(PurchaseRequestModel purchaseRequest)
        {
            if (_currentUserService.UserId != purchaseRequest.UserId)
                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to purchase");
            if (_currentUserService.UserId != null) purchaseRequest.UserId = _currentUserService.UserId.Value;
            // See if Movie is already purchased.
            if (await IsMoviePurchased(purchaseRequest))
                throw new ConflictException("Movie already Purchased");
            // Get Movie Price from Movie Table
            var movie = await _movieService.GetMovieAsync(purchaseRequest.MovieId);
            purchaseRequest.TotalPrice = movie.Price;

            var purchase = _mapper.Map<Purchase>(purchaseRequest);
            await _purchaseRepository.AddAsync(purchase);
        }

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest)
        {
            return await _purchaseRepository.GetExistsAsync(p =>
                p.UserId == purchaseRequest.UserId && p.MovieId == purchaseRequest.MovieId);
        }

        public async Task<PurchaseResponseModel> GetAllPurchasesForUser(int id)
        {
            if (_currentUserService.UserId != id)
                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to View Purchases");

            var purchasedMovies = await _purchaseRepository.ListAllWithIncludesAsync(
                p => p.UserId == _currentUserService.UserId,
                p => p.Movie);
            return _mapper.Map<PurchaseResponseModel>(purchasedMovies);
        }

        public async Task AddMovieReview(ReviewRequestModel reviewRequest)
        {
            if (_currentUserService.UserId != reviewRequest.UserId)
                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to Review");
            var review = _mapper.Map<Review>(reviewRequest);

            await _reviewRepository.AddAsync(review);
        }

        public async Task UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            if (_currentUserService.UserId != reviewRequest.UserId)
                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to Review");
            var review = _mapper.Map<Review>(reviewRequest);

            await _reviewRepository.UpdateAsync(review);
        }

        public async Task DeleteMovieReview(int userId, int movieId)
        {
            if (_currentUserService.UserId != userId)
                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to Delete Review");
            var review = await _reviewRepository.ListAsync(r => r.UserId == userId && r.MovieId == movieId);
            await _reviewRepository.DeleteAsync(review.First());
        }

        public async Task<ReviewResponseModel> GetAllReviewsByUser(int id)
        {
            if (_currentUserService.UserId != id)
                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to View Reviews");

            var userReviews = await _reviewRepository.ListAllWithIncludesAsync(r => r.UserId == id, r => r.Movie);
            return _mapper.Map<ReviewResponseModel>(userReviews);
        }
    }
}