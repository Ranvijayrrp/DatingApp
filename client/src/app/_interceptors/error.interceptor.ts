import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, catchError, of, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: any) => {
        if (error) {
          //console.error('Error occurred:', error); // Log the error for debugging

          switch (error.status) {
            case 400: // Bad Request
              // Handle specific error cases (e.g., display user-friendly message)
              if (error.error.errors) {
                const modelStateErrors = []
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modelStateErrors.push(error.error.errors[key]);
                  }
                }
                console.log(error);
                throw modelStateErrors.flat();
              } else {
                this.toastr.error(error.statusText, error.status);
                console.log(error);
              }
              break;
            case 401: // Unauthorized
              // Handle unauthorized access (e.g., redirect to login)
              //return of('Unauthorized access. Please login.');
              this.toastr.error(error.statusText, error.status);
              console.log(error);
              break;

            case 404:
              this.router.navigateByUrl('/not-found');
              console.log(error);
              break;

            case 500:
              const navigationExtras: NavigationExtras = { state: { error: error.error } };
              this.router.navigateByUrl('/server-error', navigationExtras);
              console.log(error);
              break;
            // ... other status code cases

            default:
              this.toastr.error('Something unexpected went wrong');
              console.log(error);
            //return of(error);
          }
        }

        // Unreachable in most cases, but included for completeness
        return of(error); // Default error if no specific case matches
      })

    )
  }
}
