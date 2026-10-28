import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CreateTicketRequest, CreateWorkspaceRequest, SystemInfo, TicketDto, WorkspaceDto } from './api.models';

@Injectable({ providedIn: 'root' })
export class TicketingApiService {
  private readonly apiBase = '/api/v2';

  constructor(private readonly http: HttpClient) {}

  getSystem(): Observable<SystemInfo> {
    return this.http.get<SystemInfo>(`${this.apiBase}/system`);
  }

  createWorkspace(request: CreateWorkspaceRequest): Observable<WorkspaceDto> {
    return this.http.post<WorkspaceDto>(`${this.apiBase}/workspaces`, request);
  }

  createTicket(request: CreateTicketRequest): Observable<TicketDto> {
    return this.http.post<TicketDto>(`${this.apiBase}/tickets`, request);
  }

  getTicket(ticketId: string): Observable<TicketDto> {
    return this.http.get<TicketDto>(`${this.apiBase}/tickets/${encodeURIComponent(ticketId)}`);
  }
}
