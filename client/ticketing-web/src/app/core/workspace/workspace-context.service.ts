import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class WorkspaceContextService {
  private readonly storageKey = 'ticketing.workspaceId';
  private readonly storedValue = typeof localStorage === 'undefined' ? null : localStorage.getItem(this.storageKey);

  readonly workspaceId = signal<string | null>(this.storedValue);

  setWorkspaceId(workspaceId: string | null): void {
    const normalized = workspaceId?.trim() || null;
    this.workspaceId.set(normalized);

    if (typeof localStorage === 'undefined') {
      return;
    }

    if (normalized) {
      localStorage.setItem(this.storageKey, normalized);
    } else {
      localStorage.removeItem(this.storageKey);
    }
  }
}
