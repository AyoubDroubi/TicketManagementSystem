import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { WorkspaceContextService } from './workspace-context.service';

export const workspaceHeaderInterceptor: HttpInterceptorFn = (request, next) => {
  const workspaceId = inject(WorkspaceContextService).workspaceId();

  if (!workspaceId || request.url.endsWith('/workspaces')) {
    return next(request);
  }

  return next(request.clone({ setHeaders: { 'X-Workspace-Id': workspaceId } }));
};
