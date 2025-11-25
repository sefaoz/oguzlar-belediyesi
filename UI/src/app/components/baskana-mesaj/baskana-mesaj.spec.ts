import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaskanaMesaj } from './baskana-mesaj';

describe('BaskanaMesaj', () => {
  let component: BaskanaMesaj;
  let fixture: ComponentFixture<BaskanaMesaj>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaskanaMesaj]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BaskanaMesaj);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
