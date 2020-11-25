import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styleUrls: ['./genres.component.scss']
})
export class GenresComponent implements OnInit {

  genres: any;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.http.get('https://localhost:44312/api/genres').subscribe(
      (data) => {
        console.log(data);
        this.genres = data;
      }
    );
  }

}
