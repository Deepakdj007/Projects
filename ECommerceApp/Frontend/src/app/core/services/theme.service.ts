import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly tokenKey = 'theme';

  private currentThemeSignal = signal<'light' | 'dark'>(this.detectInitialTheme());
  currentTheme = this.currentThemeSignal.asReadonly();

  constructor() {
    this.applyTheme(this.currentThemeSignal());
  }

  private detectInitialTheme(): 'light' | 'dark' {
    const stored = localStorage.getItem(this.tokenKey) as 'light' | 'dark' | null;
    if (stored === 'light' || stored === 'dark') {
      return stored;
    }

    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    return prefersDark ? 'dark' : 'light';
  }

  toggleTheme(): void {
    const newTheme = this.currentThemeSignal() === 'dark' ? 'light' : 'dark';
    this.setTheme(newTheme);
  }

  setTheme(theme: 'light' | 'dark'): void {
    console.log('[ThemeService] Setting theme:', theme);

    this.currentThemeSignal.set(theme);
    this.applyTheme(theme);
    localStorage.setItem(this.tokenKey, theme);
  }

  private applyTheme(theme: 'light' | 'dark'): void {
    document.documentElement.setAttribute('data-theme', theme);
  }
}
