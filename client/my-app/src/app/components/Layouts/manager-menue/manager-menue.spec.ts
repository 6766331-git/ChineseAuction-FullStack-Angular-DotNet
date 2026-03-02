import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagerMenue } from './manager-menue';

describe('ManagerMenue', () => {
  let component: ManagerMenue;
  let fixture: ComponentFixture<ManagerMenue>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManagerMenue]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManagerMenue);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
