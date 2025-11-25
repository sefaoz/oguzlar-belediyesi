import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Iletisim } from './iletisim';

describe('Iletisim', () => {
  let component: Iletisim;
  let fixture: ComponentFixture<Iletisim>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Iletisim]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Iletisim);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
