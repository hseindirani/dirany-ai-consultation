import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, switchMap, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { LoginRequest } from './models/login-request';
import { CurrentUser } from './models/current-user';
import { CsrfTokenResponse } from './models/csrf-token-response';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  readonly currentUser = signal<CurrentUser | null>(null);
  readonly csrfToken = signal<string | null>(null);

  login(username: string, password: string): Observable<void> {
    const request: LoginRequest = {
      username,
      password,
    };

    return this.http.post<void>(
      `${this.apiUrl}/auth/login`,
      request
    );
  }

  getCurrentUser(): Observable<CurrentUser> {
    return this.http
      .get<CurrentUser>(`${this.apiUrl}/auth/me`)
      .pipe(
        tap((user) => this.currentUser.set(user))
      );
  }

  getCsrfToken(): Observable<CsrfTokenResponse> {
    return this.http
      .get<CsrfTokenResponse>(`${this.apiUrl}/auth/csrf-token`)
      .pipe(
        tap((response) => this.csrfToken.set(response.token))
      );
  }

  logout(): Observable<void> {
    return this.http
      .post<void>(`${this.apiUrl}/auth/logout`, {})
      .pipe(
        tap(() => {
          this.currentUser.set(null);
          this.csrfToken.set(null);
        })
      );
  }
  initializeSession(): Observable<boolean> {
  return this.getCurrentUser().pipe(
    switchMap(() =>
      this.getCsrfToken().pipe(
        switchMap(() => of(true))
      )
    ),
    catchError(() => {
      this.currentUser.set(null);
      this.csrfToken.set(null);

      return of(false);
    })
  );
}
hasSession(): boolean {
  return this.currentUser() !== null && this.csrfToken() !== null;
}
clearSession(): void {
  this.currentUser.set(null);
  this.csrfToken.set(null);
}
}