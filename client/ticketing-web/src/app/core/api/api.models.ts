export type TicketPriority = 'Low' | 'Normal' | 'High' | 'Urgent';
export type TicketStatus = 'New' | 'Triage' | 'Assigned' | 'InProgress' | 'PendingRequester' | 'PendingInternal' | 'PendingThirdParty' | 'Resolved' | 'Closed' | 'Reopened' | 'Cancelled' | 'Duplicate';
export type WorkspaceStatus = 'Active' | 'Inactive' | 'Archived';

export interface SystemInfo {
  service: string;
  version: string;
  utcNow: string;
}

export interface WorkspaceDto {
  id: string;
  name: string;
  slug: string;
  status: WorkspaceStatus;
  createdAtUtc: string;
  updatedAtUtc: string;
}

export interface CreateWorkspaceRequest {
  name: string;
  slug: string;
}

export interface CreateTicketRequest {
  requesterId: string;
  summary: string;
  description: string | null;
  priority: TicketPriority;
}

export interface TicketDto extends CreateTicketRequest {
  id: string;
  workspaceId: string;
  status: TicketStatus;
  assignedTeamId: string | null;
  assignedAgentId: string | null;
  resolution: string | null;
  createdAtUtc: string;
  updatedAtUtc: string;
  resolvedAtUtc: string | null;
  closedAtUtc: string | null;
}
