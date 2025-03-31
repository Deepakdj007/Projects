import { Component, effect, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-auth-layout',
  imports: [RouterOutlet],
  templateUrl: './auth-layout.component.html',
  styleUrl: './auth-layout.component.scss'
})
export class AuthLayoutComponent {
  images = [
    {
      url: '/assets/models/1.png',
      title: 'Urban Essentials',
      description: 'Minimalist designs that blend city energy with comfort.'
    },
    {
      url: '/assets/models/2.png',
      title: 'Monochrome Muse',
      description: 'Effortless elegance in clean, timeless neutrals.'
    },
    {
      url: '/assets/models/3.png',
      title: 'Street Luxe',
      description: 'Bold cuts and luxurious layers for everyday confidence.'
    },
    {
      url: '/assets/models/4.png',
      title: 'Natural Flow',
      description: 'Lightweight pieces made for movement and grace.'
    }
  ];

  currentIndex = signal(0);

  constructor() {
    effect(() => {
      const interval = setInterval(() => {
        const next = (this.currentIndex() + 1) % this.images.length;
        this.currentIndex.set(next);
      }, 3500); // 3.5 sec delay
      return () => clearInterval(interval); // cleanup
    });
  }
}
