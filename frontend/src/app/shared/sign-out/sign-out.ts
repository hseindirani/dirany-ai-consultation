import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';

import { Auth } from '../../core/auth/auth';

@Component({
  selector: 'app-sign-out',
  imports: [],
  templateUrl: './sign-out.html',
  styleUrl: './sign-out.css',
})
export class SignOut {
  private readonly auth = inject(Auth);
  private readonly router = inject(Router);

  readonly isSigningOut = signal(false);

  signOut(): void {
    if (this.isSigningOut()) {
      return;
    }

    this.isSigningOut.set(true);

    this.auth.logout().subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: () => {
        this.isSigningOut.set(false);
      },
    });
  }
}