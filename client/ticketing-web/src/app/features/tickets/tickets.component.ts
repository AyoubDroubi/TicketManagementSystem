import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TicketPriority, TicketDto, WorkspaceDto } from '../../core/api/api.models';
import { TicketingApiService } from '../../core/api/ticketing-api.service';
import { WorkspaceContextService } from '../../core/workspace/workspace-context.service';

@Component({
  selector: 'app-tickets',
  imports: [FormsModule],
  template: `
    <header class="page-heading">
      <div><p class="eyebrow">FOUNDATION CONSOLE</p><h1>Tickets</h1></div>
      <span class="workspace">Workspace: {{ workspaceContext.workspaceId() ?? 'none' }}</span>
    </header>

    <div class="layout">
      <section class="panel">
        <h2>Create workspace</h2>
        <label>Name <input [(ngModel)]="workspaceName" placeholder="Acme Support"></label>
        <label>Slug <input [(ngModel)]="workspaceSlug" placeholder="acme-support"></label>
        <button type="button" (click)="createWorkspace()">Create and select</button>
        @if (createdWorkspace()) { <p class="success">Created {{ createdWorkspace()!.name }}</p> }
      </section>

      <section class="panel">
        <h2>Create ticket</h2>
        <label>Requester ID <input [(ngModel)]="requesterId" placeholder="UUID"></label>
        <label>Summary <input [(ngModel)]="summary" placeholder="What needs attention?"></label>
        <label>Description <textarea [(ngModel)]="description" rows="4"></textarea></label>
        <label>Priority
          <select [(ngModel)]="priority">
            <option value="Low">Low</option><option value="Normal">Normal</option><option value="High">High</option><option value="Urgent">Urgent</option>
          </select>
        </label>
        <button type="button" (click)="createTicket()" [disabled]="!workspaceContext.workspaceId()">Create ticket</button>
      </section>
    </div>

    @if (message()) { <p class="message">{{ message() }}</p> }
    @if (ticket()) {
      <article class="ticket-card">
        <div><span class="badge">{{ ticket()!.status }}</span><span class="badge">{{ ticket()!.priority }}</span></div>
        <h2>{{ ticket()!.summary }}</h2>
        <p>{{ ticket()!.description || 'No description' }}</p>
        <small>{{ ticket()!.id }}</small>
      </article>
    }
  `,
  styles: [`
    .page-heading { display: flex; align-items: end; justify-content: space-between; gap: 16px; margin-bottom: 24px; }
    .eyebrow { color: var(--accent); font-weight: 800; letter-spacing: .12em; font-size: .75rem; margin: 0 0 6px; }
    h1, h2 { margin: 0; } .workspace { color: var(--muted); font-size: .9rem; overflow-wrap: anywhere; }
    .layout { display: grid; grid-template-columns: repeat(2, minmax(0, 1fr)); gap: 16px; }
    .panel, .ticket-card { border: 1px solid var(--border); border-radius: 16px; background: var(--surface); padding: 20px; }
    .panel { display: grid; gap: 14px; } label { display: grid; gap: 6px; color: var(--muted); font-size: .9rem; }
    input, textarea, select { width: 100%; box-sizing: border-box; font: inherit; padding: 11px 12px; border: 1px solid var(--border); border-radius: 10px; background: var(--background); color: var(--text); }
    button { justify-self: start; border: 0; border-radius: 10px; padding: 11px 14px; background: var(--text); color: var(--surface); font-weight: 700; cursor: pointer; }
    button:disabled { opacity: .45; cursor: not-allowed; } .success { color: var(--success); margin: 0; }
    .message { padding: 12px 14px; background: var(--surface-muted); border-radius: 12px; }
    .ticket-card { margin-top: 16px; } .ticket-card h2 { margin: 12px 0 8px; } .ticket-card p, .ticket-card small { color: var(--muted); }
    .badge { display: inline-flex; margin-right: 8px; padding: 5px 8px; border-radius: 999px; background: var(--surface-muted); font-size: .78rem; }
    @media (max-width: 760px) { .layout { grid-template-columns: 1fr; } .page-heading { align-items: start; flex-direction: column; } }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TicketsComponent {
  workspaceName = '';
  workspaceSlug = '';
  requesterId = '';
  summary = '';
  description = '';
  priority: TicketPriority = 'Normal';

  readonly createdWorkspace = signal<WorkspaceDto | null>(null);
  readonly ticket = signal<TicketDto | null>(null);
  readonly message = signal('');

  constructor(
    private readonly api: TicketingApiService,
    readonly workspaceContext: WorkspaceContextService
  ) {}

  createWorkspace(): void {
    this.message.set('');
    this.api.createWorkspace({ name: this.workspaceName, slug: this.workspaceSlug }).subscribe({
      next: (workspace) => {
        this.createdWorkspace.set(workspace);
        this.workspaceContext.setWorkspaceId(workspace.id);
      },
      error: () => this.message.set('Workspace creation failed. Check the API response and validation rules.')
    });
  }

  createTicket(): void {
    this.message.set('');
    this.api.createTicket({ requesterId: this.requesterId, summary: this.summary, description: this.description || null, priority: this.priority }).subscribe({
      next: (ticket) => this.ticket.set(ticket),
      error: () => this.message.set('Ticket creation failed. Confirm workspace, requester UUID, and API availability.')
    });
  }
}
