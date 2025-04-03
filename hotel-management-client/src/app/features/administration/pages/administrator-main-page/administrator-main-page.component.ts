import {Component, OnInit} from '@angular/core';
import {MatDivider} from '@angular/material/divider';
import {MatListItem, MatNavList} from '@angular/material/list';
import {MatSidenav, MatSidenavContainer, MatSidenavContent} from '@angular/material/sidenav';
import {ActivatedRoute, Router, RouterLink, RouterLinkActive, RouterOutlet} from '@angular/router';

@Component({
  selector: 'app-administrator-main-page',
  imports: [
    MatDivider,
    MatListItem,
    MatNavList,
    MatSidenav,
    MatSidenavContainer,
    MatSidenavContent,
    RouterLinkActive,
    RouterOutlet,
    RouterLink
  ],
  templateUrl: './administrator-main-page.component.html',
  styleUrl: './administrator-main-page.component.scss'
})
export class AdministratorMainPageComponent implements OnInit {
  constructor(public router: Router, public route: ActivatedRoute) {
  }
  ngOnInit() {
    if(this.router.url === "/Administrator"){
      this.router.navigate(['./users'], {relativeTo: this.route});
    }else{
      console.log(this.router.url);
    }
  }
}
