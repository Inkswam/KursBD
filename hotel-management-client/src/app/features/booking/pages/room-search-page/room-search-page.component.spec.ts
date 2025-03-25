import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomSearchPageComponent } from './room-search-page.component';

describe('RoomSearchPageComponent', () => {
  let component: RoomSearchPageComponent;
  let fixture: ComponentFixture<RoomSearchPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoomSearchPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoomSearchPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
