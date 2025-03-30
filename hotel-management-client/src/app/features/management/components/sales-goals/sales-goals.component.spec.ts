import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalesGoalsComponent } from './sales-goals.component';

describe('SalesGoalsComponent', () => {
  let component: SalesGoalsComponent;
  let fixture: ComponentFixture<SalesGoalsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SalesGoalsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SalesGoalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
