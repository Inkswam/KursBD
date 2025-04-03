import {Component, OnInit} from '@angular/core';
import {MatInput, MatLabel} from "@angular/material/input";
import {MatToolbar} from "@angular/material/toolbar";
import {NgForOf} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatFormField} from '@angular/material/form-field';
import {User} from '../../../../shared/models/user.model';
import {ManagementService} from '../../management.service';
import {BlacklistedUserComponent} from '../../components/blacklisted-user/blacklisted-user.component';

@Component({
  selector: 'app-blacklist-page',
  imports: [
    MatFormField,
    MatInput,
    MatLabel,
    MatToolbar,
    NgForOf,
    ReactiveFormsModule,
    FormsModule,
    BlacklistedUserComponent
  ],
  templateUrl: './blacklist-page.component.html',
  styleUrl: './blacklist-page.component.scss'
})
export class BlacklistPageComponent implements OnInit{
  public searchName: string;
  public searchEmail: string;
  public searchPhone: string;

  public allUsers: User[];
  public users: User[];

  constructor(public managementService: ManagementService){
    this.searchName = "";
    this.searchEmail = "";
    this.searchPhone = "";

    this.allUsers = []
    this.users = [];
  }

  ngOnInit() {
    this.getBlacklistedUsers();
  }

  getBlacklistedUsers() {
    this.managementService.getBlacklistedUsers().subscribe({
      next: users => {
        this.allUsers = [];
        for(let user of users){
          this.allUsers.push(user);
        }

        this.applyFilters();
      },
      error: error => console.log(error),
    })
  }

  applyFilters() {
    this.users = this.allUsers
      .filter(
        (u) =>
          (!this.searchName ||
            u.firstName.toLowerCase().includes(this.searchName.toLowerCase()) ||
            u.lastName.toLowerCase().includes(this.searchName.toLowerCase()) ||
            (u.firstName.toLowerCase() + " " + u.lastName.toLowerCase()).includes(this.searchName.toLowerCase())) &&
          (!this.searchEmail || u.email.toLowerCase().includes(this.searchEmail.toLowerCase())) &&
          (!this.searchPhone || u.phoneNumber.includes(this.searchPhone))
      );

    console.log(this.users);
  }
}
