import { TestBed } from '@angular/core/testing';

import { GiftManagement } from './gift-management';

describe('GiftManagement', () => {
  let service: GiftManagement;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GiftManagement);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
