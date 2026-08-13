import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TicketingApiService } from '../../core/api/ticketing-api.service';
import { WorkspaceContextService } from '../../core/workspace/workspace-context.service';

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  template: `
    <section class="hero">
      <p class="eyebrow">SERVICE DESK V2</p>
      <h1>One operational queue for every customer request.</h1>
      <p class="lede">The V2 foundation is API-first, workspace-aware, and designed to grow into queues, SLA, routing, automation, knowledge, and ITSM workflows.</p>
      <div class="actions"><a class="button primary" routerLink="/tickets">Open ticket workspace</a></div>
    </section>

    <section class="grid" aria-label="Foundation status">
      <article><span>API</span><strong>{{ apiStatus() }}</strong><small>/api/v2</small></article>
      <article><span>Workspace</span><strong>{{ workspaceContext.workspaceId() ?? 'Not selected' }}</strong><small>Temporary pre-auth context</small></article>
      <article><span>Architecture</span><strong>Modular Monolith</strong><small>DDD + CQRS boundaries</small></article>
    </section>
  `,
  styles: [`
    .hero { padding: 52px 0 36px; max-width: 820px; }
    .eyebrow { font-size: .78rem; letter-spacing: .14em; font-weight: 800; color: var(--accent); }
    h1 { font-size: clamp(2.3rem, 6vw, 4.8rem); line-height: .98; margin: 12px 0 20px; letter-spacing: -.04em; }
    .lede { color: var(--muted); font-size: 1.08rem; line-height: 1.7; max-width: 720px; }
    .actions { margin-top: 28px; }
    .button { display: inline-flex; padding: 12px 16px; border-radius: 12px; text-decoration: none; font-weight: 700; }
    .primary { background: var(--text); color: var(--surface); }
    .grid { display: grid; grid-template-columns: repeat(3, minmax(0, 1fr)); gap: 14px; margin-top: 32px; }
    article { padding: 20px; border: 1px solid var(--border); border-radius: 16px; background: var(--surface); display: grid; gap: 8px; }
    article span, article small { color: var(--muted); } article strong { overflow-wrap: anywhere; }
    @media (max-width: 760px) { .grid { grid-template-columns: 1fr; } }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit {
  readonly apiStatus = signal('Checking…');

  constructor(
    private readonly api: TicketingApiService,
    readonly workspaceContext: WorkspaceContextService
  ) {}

  ngOnInit(): void {
    this.api.getSystem().subscribe({
      next: (system) => this.apiStatus.set(`${system.service} ${system.version}`),
      error: () => this.apiStatus.set('API unavailable')
    });
  }
}
