import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceptionistManagementPageComponent } from './receptionist-management-page.component';

describe('ReceptionistManagementPageComponent', () => {
  let component: ReceptionistManagementPageComponent;
  let fixture: ComponentFixture<ReceptionistManagementPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReceptionistManagementPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReceptionistManagementPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
