import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Movie } from 'src/app/shared/models/movie';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  constructor(private apiService:ApiService) { }

  getTopRevenuMovies(): Observable<Movie[]> {
    return this.apiService.getAll('movies/toprevenue');
  }
  getMovieDetails(id: number): Observable<Movie> {
    return this.apiService.getOne('movies',id);
  }

  getMovieByGenre(id: number): Observable<Movie[]> {
    return this.apiService.getAll('movies/genre', id);
  }
}
