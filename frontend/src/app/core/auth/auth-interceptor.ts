import { inject } from "@angular/core";
import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";

import { Auth } from "./auth";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(Auth);
  const router = inject(Router);
  req = req.clone({
    withCredentials: true,
  });

  const unsafeMethods = ["POST", "PUT", "PATCH", "DELETE"];
  const isUnsafeRequest = unsafeMethods.includes(req.method);
  const isLoginRequest = req.url.endsWith("/auth/login");

  const csrfToken = auth.csrfToken();

  if (isUnsafeRequest && !isLoginRequest && csrfToken) {
    req = req.clone({
      setHeaders: {
        "X-CSRF-TOKEN": csrfToken,
      },
    });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !isLoginRequest) {
        auth.clearSession();
        router.navigate(["/login"]);
      }

      return throwError(() => error);
    }),
  );
};
