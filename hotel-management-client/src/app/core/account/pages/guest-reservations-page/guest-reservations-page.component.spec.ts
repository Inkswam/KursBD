import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GuestReservationsPageComponent } from './guest-reservations-page.component';

describe('GuestReservationsPageComponent', () => {
  let component: GuestReservationsPageComponent;
  let fixture: ComponentFixture<GuestReservationsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GuestReservationsPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GuestReservationsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
