import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BlacklistedUserComponent } from './blacklisted-user.component';

describe('BlacklistedUserComponent', () => {
  let component: BlacklistedUserComponent;
  let fixture: ComponentFixture<BlacklistedUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BlacklistedUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BlacklistedUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
