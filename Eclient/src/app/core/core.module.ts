import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarModule } from './nav-bar/nav-bar.module';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { TestErrorComponent } from './test-error/test-error.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { ToastrModule } from 'ngx-toastr';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { BreadcrumbModule } from 'xng-breadcrumb';



@NgModule({
  declarations: [
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    SectionHeaderComponent
  ],
  imports: [
    CommonModule,
    NavBarModule,
    BreadcrumbModule,
    ToastrModule.forRoot({
      positionClass:'toast-bottom-right',
      preventDuplicates:true
    })
  ],
  exports: [
    NavBarModule,
    SectionHeaderComponent
  //  NavBarComponent,
   // SectionHeaderComponent
  ]
})
export class CoreModule { }
