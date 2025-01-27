import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { delay, finalize } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busyService:BusyService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  if(request.method==='POST' && request.url.includes('order')){
    return next.handle(request);
  }
  if(request.method==='DELETE'){
    return next.handle(request);
  }
  
    if(request.url.includes('emailexists')){
      return next.handle(request);
  }
  this.busyService.busy();
    return next.handle(request).pipe(
     // delay(500),
      finalize(()=> {
        this.busyService.idle();
      })
    );
  }
}
