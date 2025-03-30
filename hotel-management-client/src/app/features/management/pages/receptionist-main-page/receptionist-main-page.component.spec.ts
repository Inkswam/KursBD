import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceptionistMainPageComponent } from './receptionist-main-page.component';

describe('ReceptionistMainPageComponent', () => {
  let component: ReceptionistMainPageComponent;
  let fixture: ComponentFixture<ReceptionistMainPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReceptionistMainPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReceptionistMainPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
