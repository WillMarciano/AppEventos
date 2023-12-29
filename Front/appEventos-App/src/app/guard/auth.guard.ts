import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);
  const toaster = inject(ToastrService);

  if (localStorage.getItem('user') !== null) {
    return true;
  } else {
    toaster.info('Usuário não autenticado!');
    router.navigate(['/usuario/login']);
    return false;
  }
};
