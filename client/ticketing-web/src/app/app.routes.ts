import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/home/home.component').then((module) => module.HomeComponent)
  },
  {
    path: 'tickets',
    loadComponent: () => import('./features/tickets/tickets.component').then((module) => module.TicketsComponent)
  },
  { path: '**', redirectTo: '' }
];
