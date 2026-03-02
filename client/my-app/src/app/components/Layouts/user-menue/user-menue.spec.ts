import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserMenue } from './user-menue';

describe('ManagerMenue', () => {
  let component: UserMenue;
  let fixture: ComponentFixture<UserMenue>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserMenue]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserMenue);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
