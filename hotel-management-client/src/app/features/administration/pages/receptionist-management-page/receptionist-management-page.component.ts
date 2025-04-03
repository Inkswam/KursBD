import {Component, OnInit} from '@angular/core';
import {MatInput, MatLabel} from '@angular/material/input';
import {MatToolbar} from '@angular/material/toolbar';
import {NgForOf} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MatFormField} from '@angular/material/form-field';
import {User} from '../../../../shared/models/user.model';
import {ManagementService} from '../../../management/management.service';
import {AssignableUserComponent} from '../../components/assignable-user/assignable-user.component';
import {AdminService} from '../../admin.service';

@Component({
  selector: 'app-receptionist-management-page',
  imports: [
    MatFormField,
    MatInput,
    MatLabel,
    MatToolbar,
    NgForOf,
    ReactiveFormsModule,
    FormsModule,
    AssignableUserComponent
  ],
  templateUrl: './receptionist-management-page.component.html',
  styleUrl: './receptionist-management-page.component.scss'
})
export class ReceptionistManagementPageComponent implements OnInit{

  public searchName: string;
  public searchEmail: string;
  public searchPhone: string;

  public allUsers: User[];
  public users: User[];

  constructor(public adminService: AdminService){
    this.searchName = "";
    this.searchEmail = "";
    this.searchPhone = "";

    this.allUsers = []
    this.users = [];
  }

  ngOnInit() {
    this.getAllUsers();
  }

  getAllUsers() {
    this.adminService.getAllUsers().subscribe({
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
