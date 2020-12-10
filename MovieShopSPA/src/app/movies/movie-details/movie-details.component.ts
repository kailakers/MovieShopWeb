import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MovieService } from 'src/app/core/services/movie.service';
import { Movie } from 'src/app/shared/models/movie';
@Component({
  selector: 'app-movie-details',
  templateUrl: './movie-details.component.html',
  styleUrls: ['./movie-details.component.css'],
})
export class MovieDetailsComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService
  ) {}
  movieId?: number;
  movie?: Movie;
  ngOnInit(): void {
    this.route.paramMap.subscribe((p) => {
      this.movieId = +p.get('id');
      console.log(this.movieId);
      // make a call to movie service to get movie details
      this.movieService.getMovieDetails(this.movieId).subscribe((m) => {
        this.movie = m;
        console.log(this.movie);
      });
    });
  }
}