import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InterceptorService implements HttpInterceptor {
  constructor(private auth: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (this.auth.loggedIn) {
      const token = localStorage.getItem('token');
      const tokenReq = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` },
      });
      return next.handle(tokenReq);
    } else {
      const tokenReq = req.clone();
      return next.handle(tokenReq);
    }
  }
}
