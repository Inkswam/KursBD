import {Component, OnInit} from '@angular/core';
import {MatSidenav, MatSidenavContainer, MatSidenavContent} from '@angular/material/sidenav';
import {MatDivider, MatListItem, MatNavList} from '@angular/material/list';
import {DashboardComponent} from '../dashboard/dashboard.component';
import {ActivatedRoute, Router, RouterLink, RouterLinkActive} from '@angular/router';

@Component({
  selector: 'app-receptionist-main-page',
  imports: [
    MatSidenav,
    MatSidenavContainer,
    MatSidenavContent,
    MatNavList,
    MatListItem,
    MatDivider,
    DashboardComponent,
    RouterLink,
    RouterLinkActive,
  ],
  templateUrl: './receptionist-main-page.component.html',
  styleUrl: './receptionist-main-page.component.scss'
})
export class ReceptionistMainPageComponent implements OnInit {

  constructor(public router: Router, public route: ActivatedRoute) {
  }
  ngOnInit() {
    if(this.router.url === "/Receptionist"){
      this.router.navigate(['./dashboard'], {relativeTo: this.route});
    }else{
      console.log(this.router.url);
    }
  }
}
