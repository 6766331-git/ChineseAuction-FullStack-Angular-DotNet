import { TestBed } from '@angular/core/testing';

import { Purchase } from './purchase';

describe('Purchase', () => {
  let service: Purchase;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Purchase);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
