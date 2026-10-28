import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  template: `
    <header class="app-header">
      <a class="brand" routerLink="/">Ticketing</a>
      <nav aria-label="Primary navigation">
        <a routerLink="/" routerLinkActive="active" [routerLinkActiveOptions]="{ exact: true }">Overview</a>
        <a routerLink="/tickets" routerLinkActive="active">Tickets</a>
      </nav>
    </header>
    <main class="app-main"><router-outlet /></main>
  `,
  styles: [`
    :host { display: block; min-height: 100vh; }
    .app-header { min-height: 64px; display: flex; align-items: center; justify-content: space-between; gap: 24px; padding: 0 32px; border-bottom: 1px solid var(--border); background: var(--surface); position: sticky; top: 0; z-index: 10; }
    .brand { color: var(--text); font-size: 1.05rem; font-weight: 800; text-decoration: none; }
    nav { display: flex; gap: 8px; }
    nav a { color: var(--muted); text-decoration: none; padding: 8px 12px; border-radius: 10px; }
    nav a:hover, nav a.active { color: var(--text); background: var(--surface-muted); }
    .app-main { max-width: 1180px; margin: 0 auto; padding: 32px; }
    @media (max-width: 640px) { .app-header { padding: 0 16px; } .app-main { padding: 20px 16px; } }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AppComponent {}
