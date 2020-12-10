import { Component, Input, OnInit } from '@angular/core';
import { MoviesComponent } from 'src/app/movies/movies.component';
import { Movie } from '../../models/movie';

@Component({
  selector: 'app-movie-card',
  templateUrl: './movie-card.component.html',
  styleUrls: ['./movie-card.component.css']
})
export class MovieCardComponent implements OnInit {

  @Input() movie?: Movie;
  constructor() { }

  ngOnInit(): void {
  }

}
