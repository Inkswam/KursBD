import {AfterContentInit, Component, OnInit} from '@angular/core';
import {MatToolbar} from '@angular/material/toolbar';
import {RouterLink, RouterLinkActive} from '@angular/router';
import {MatAnchor, MatButton, MatFabButton} from '@angular/material/button';
import {AuthenticationService} from '../../authentication/authentication.service';
import {MatIcon} from '@angular/material/icon';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-header',
  imports: [
    MatToolbar,
    MatAnchor,
    MatIcon,
    RouterLink,
    MatFabButton,
    MatButton,
    RouterLinkActive,
    NgIf,
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent implements AfterContentInit {


  constructor(public authenticationService: AuthenticationService) { }

  ngAfterContentInit(): void {

  }
}
