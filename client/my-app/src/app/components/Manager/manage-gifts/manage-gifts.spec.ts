import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageGifts } from './manage-gifts';

describe('ManageGifts', () => {
  let component: ManageGifts;
  let fixture: ComponentFixture<ManageGifts>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageGifts]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageGifts);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
