import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MovieService } from '../core/services/movie.service';
import { Movie } from '../shared/models/movie';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit {
  movies: Movie[] = [];
  genreId?: number;
  constructor(
    private movieService: MovieService,
    private route: ActivatedRoute
    ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((p)=>{
      this.genreId = +p.get('id');
      console.log(this.genreId);
      this.movieService.getMovieByGenre(this.genreId).subscribe((m)=>{
        this.movies = m;
        console.log(this.movies);
      })
    });
  }

}
