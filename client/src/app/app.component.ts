import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {

  title = 'client';
  httpClient = inject(HttpClient);
  user: any;
  ngOnInit(): void {
    this.httpClient.get("https://localhost:5001/api/users").subscribe({
      next: (response) => {
        this.user = response;
      },
      error: (error) => {
        console.log(error)

      },
      complete: () => {
        console.log('Request has completed')
      }
    });
  }
}
