using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MovieShop.Core.Entities;
using MovieShop.Core.Helpers;
using MovieShop.Core.Models;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;

namespace MovieShop.Infrastructure.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMovieService _movieService;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IAsyncRepository<Favorite> _favoriteRepository;
        private readonly IAsyncRepository<Review> _reviewRepository;

        public UserService(IUserRepository userRepository, ICryptoService cryptoService, 
            ICurrentUserService currentUserService, IMovieService movieService, IPurchaseRepository purchaseRepository,
            IAsyncRepository<Favorite> favoriteRepository, IAsyncRepository<Review> reviewRepository)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _currentUserService = currentUserService;
            _movieService = movieService;
            _purchaseRepository = purchaseRepository;
            _favoriteRepository = favoriteRepository;
            _reviewRepository = reviewRepository;
        }
        public async Task<UserLoginResponseModel> ValidateUser(string email, string password)
        {
            // Check if the email exists in the DB
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return null;
            
            var hashedPassword = _cryptoService.HashPassword(password, user.Salt);
            var isSuccess = user.HashedPassword == hashedPassword;
            

            var response = new UserLoginResponseModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Roles = new List<String>()
            };

            foreach (var userRole in user.UserRoles)
            {
                response.Roles.Add(userRole.Role.Name);
            }
            return isSuccess ? response : null;
        }

        public async Task<UserRegisterResponseModel> CreateUser(UserRegisterRequestModel requestModel)
        {
            //Make sure email does not exist in DB
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);
            if (dbUser != null &&
                string.Equals(dbUser.Email, requestModel.Email, StringComparison.CurrentCultureIgnoreCase))
                throw new Exception("Email Already Exist");
            
            //Create unique salt and hash the pw
            var salt = _cryptoService.CreateSalt();
            var hashedPassWord = _cryptoService.HashPassword(requestModel.Password, salt);
            var user = new User
            {
                Email = requestModel.Email,
                Salt = salt,
                HashedPassword = hashedPassWord,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName
            };
            var createdUser = await _userRepository.AddAsync(user);
            var response = new UserRegisterResponseModel
            {
                Id = createdUser.Id, 
                Email = createdUser.Email, 
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName
            };
            return response;
        }

        public Task<UserRegisterResponseModel> GetUserDetails(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetUser(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user;
        }

        public Task<PagedResultSet<User>> GetAllUsersByPagination(int pageSize = 20, int page = 0, string lastName = "")
        {
            throw new System.NotImplementedException();
        }

        public async Task AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            // check whether the movie has been added
            if (await FavoriteExists(favoriteRequest.userId, favoriteRequest.movieId)) 
                throw new Exception("The Movie has been added");
            Favorite favorite = new Favorite
            {
                UserId = favoriteRequest.userId,
                MovieId = favoriteRequest.movieId
            };
            await _favoriteRepository.AddAsync(favorite);
        }

        public async Task RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            if(await FavoriteExists(favoriteRequest.userId, favoriteRequest.movieId)==false)
                throw new Exception("The Movie has not been favorite!");

            var favorite = await _favoriteRepository.ListAsync(f => f.UserId == favoriteRequest.userId &&
                                                                   f.MovieId == favoriteRequest.movieId);
            
            await _favoriteRepository.DeleteAsync(favorite.First());
        }

        public async Task<bool> FavoriteExists(int id, int movieId)
        {
            return await _favoriteRepository.GetExistsAsync(f => f.MovieId == movieId && f.UserId == id);
        }

        public async Task<IEnumerable<FavoriteResponseModel>> GetAllFavoritesForUser(int id)
        {
            var favorites = await _favoriteRepository.ListAllWithInclude(f => f.UserId == id,
                f => f.Movie);
            var favoriteResponseModel = new List<FavoriteResponseModel>();
            foreach (var favorite in favorites)
            {
                favoriteResponseModel.Add(new FavoriteResponseModel
                {
                    UserId = favorite.UserId,
                    MovieId = favorite.MovieId
                });
            }

            return favoriteResponseModel;
        }

        public async Task PurchaseMovie(PurchaseRequestModel purchaseRequest)
        {
            //Validate Authorization
            // if(_currentUserService.UserId!=purchaseRequest.UserId)
            //     throw new Exception("You are not authorized to purchase the movie!");
            //Check whether the movie is purchased
            if(await IsMoviePurchased(purchaseRequest))
                throw new Exception("The movie has been purchased");
            var movie = await _movieService.GetMovieAsync(purchaseRequest.MovieId);
            purchaseRequest.TotalPrice = movie.Price;
            purchaseRequest.PurchaseDateTime = DateTime.Now;
            purchaseRequest.PurchaseNumber = Guid.NewGuid();
            Purchase purchase = new Purchase
            {
                UserId = purchaseRequest.UserId,
                MovieId = purchaseRequest.MovieId,
                PurchaseNumber = purchaseRequest.PurchaseNumber,
                PurchaseDateTime = purchaseRequest.PurchaseDateTime,
                TotalPrice = purchaseRequest.TotalPrice
            };
            
            await _purchaseRepository.AddAsync(purchase);

        }

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest)
        {
            return await _purchaseRepository.GetExistsAsync(p => p.MovieId == purchaseRequest.MovieId &&
                                                    p.UserId == purchaseRequest.UserId);
        }

        public async Task<IEnumerable<PurchaseResponseModel>> GetAllPurchasesForUser(int id)
        {
            var purchases = await _purchaseRepository.GetPurchasesByUserId(id);
            var purchaseResponseModel = new List<PurchaseResponseModel>();
            foreach (var purchase in purchases)
            {
                purchaseResponseModel.Add(new PurchaseResponseModel
                {
                    UserId = purchase.UserId,
                    PurchaseNumber = purchase.PurchaseNumber,
                    TotalPrice = purchase.TotalPrice,
                    PurchaseDateTime = purchase.PurchaseDateTime,
                    MovieId = purchase.MovieId
                });
            }

            return purchaseResponseModel;
        }

        public async Task<ReviewResponseModel> AddMovieReview(ReviewRequestModel reviewRequest)
        {
            Review review = new Review
            {
                UserId = reviewRequest.UserId,
                MovieId = reviewRequest.MovieId,
                ReviewText = reviewRequest.ReviewText,
                Rating = reviewRequest.Rating
            };
            await _reviewRepository.AddAsync(review);
            var response = new ReviewResponseModel
            {
                UserId = review.UserId,
                MovieId = review.MovieId,
                ReviewText = review.ReviewText,
                Rating = review.Rating
            };
            return response;
        }

        public async Task<ReviewResponseModel> UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            Review review = new Review
            {
                UserId = reviewRequest.UserId,
                MovieId = reviewRequest.MovieId,
                ReviewText = reviewRequest.ReviewText,
                Rating = reviewRequest.Rating
            };
            await _reviewRepository.UpdateAsync(review);
            var response = new ReviewResponseModel
            {
                UserId = review.UserId,
                MovieId = review.MovieId,
                ReviewText = review.ReviewText,
                Rating = review.Rating
            };
            return response;
        }

        public async Task DeleteMovieReview(int userId, int movieId)
        {
            var review = await _reviewRepository.ListAsync(r => r.UserId == userId &&
                                                          r.MovieId == movieId);
            
            await _reviewRepository.DeleteAsync(review.First());
        }

        public async Task<IEnumerable<ReviewResponseModel>> GetAllReviewsForUser(int id)
        {
            var reviews = await _reviewRepository.ListAllWithInclude(r => r.UserId == id, 
                r => r.Movie);

            var reviewResponseModel = new List<ReviewResponseModel>();
            foreach (var review in reviews)
            {
                reviewResponseModel.Add(new ReviewResponseModel
                {
                    UserId = review.UserId,
                    MovieId = review.MovieId,
                    ReviewText = review.ReviewText,
                    Rating = review.Rating
                });
            }

            return reviewResponseModel;
        }
    }
}