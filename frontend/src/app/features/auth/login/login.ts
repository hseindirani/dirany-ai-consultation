import { Component, inject, OnInit, signal } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { switchMap } from "rxjs";

import { Auth } from "../../../core/auth/auth";

@Component({
  selector: "app-login",
  imports: [ReactiveFormsModule],
  templateUrl: "./login.html",
  styleUrl: "./login.css",
})
export class Login implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly auth = inject(Auth);
  private readonly router = inject(Router);

  readonly isSubmitting = signal(false);
  readonly loginFailed = signal(false);

  readonly form = this.formBuilder.nonNullable.group({
    username: ["", Validators.required],
    password: ["", Validators.required],
  });

  submit(): void {
    if (this.form.invalid || this.isSubmitting()) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.loginFailed.set(false);

    const { username, password } = this.form.getRawValue();

    this.auth
      .login(username, password)
      .pipe(switchMap(() => this.auth.initializeSession()))
      .subscribe({
        next: (isAuthenticated) => {
          this.isSubmitting.set(false);

          if (!isAuthenticated) {
            this.loginFailed.set(true);
            return;
          }

          this.router.navigate(["/consultation/new"]);
        },
        error: () => {
          this.isSubmitting.set(false);
          this.loginFailed.set(true);
        },
      });
  }
  ngOnInit(): void {
    if (this.auth.hasSession()) {
      this.router.navigate(["/consultation/new"]);
      return;
    }

    this.auth.initializeSession().subscribe((isAuthenticated) => {
      if (isAuthenticated) {
        this.router.navigate(["/consultation/new"]);
      }
    });
  }
}
