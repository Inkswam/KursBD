import {Component, OnInit} from '@angular/core';
import {MatDivider} from "@angular/material/divider";
import {MatListItem, MatNavList} from "@angular/material/list";
import {MatSidenav, MatSidenavContainer, MatSidenavContent} from "@angular/material/sidenav";
import {ActivatedRoute, Router, RouterLink, RouterLinkActive, RouterOutlet} from "@angular/router";
import {EUserRole} from '../../../../shared/enums/euser-role.enum';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-account-page',
  imports: [
    MatDivider,
    MatListItem,
    MatNavList,
    MatSidenav,
    MatSidenavContainer,
    MatSidenavContent,
    RouterLinkActive,
    RouterOutlet,
    RouterLink,
    NgIf
  ],
  templateUrl: './account-page.component.html',
  styleUrl: './account-page.component.scss'
})
export class AccountPageComponent implements OnInit {
  constructor(public router: Router, public route: ActivatedRoute) {
  }
  ngOnInit() {
    if(this.router.url === "/account"){
      this.router.navigate(['./account-details'], {relativeTo: this.route});
    }else{
      console.log(this.router.url);
    }
  }

  protected readonly sessionStorage = sessionStorage;
  protected readonly EUserRole = EUserRole;
  protected readonly JSON = JSON;
}
