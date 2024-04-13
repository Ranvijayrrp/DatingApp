import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from "@angular/router";
import { AccountService } from "../_services/account.service";
import { Observable, catchError, map } from "rxjs";
import { ToastrService } from "ngx-toastr";

export const canActivateTeam: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);
  const router = inject(Router);

  return accountService.currentUser$.pipe(
    map((user) => {
      if (user) {
        console.log(user);
        return true; // User is authenticated, allow access
        
      }
       else {
        toastr.error('You shall not pass!'); // Show an error message
        return false; // User is not authenticated, deny access
      }
    }),
    //Error('error occured', error=> return error)
  );
};
