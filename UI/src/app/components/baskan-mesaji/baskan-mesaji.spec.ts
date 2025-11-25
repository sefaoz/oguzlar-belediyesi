import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaskanMesaji } from './baskan-mesaji';

describe('BaskanMesaji', () => {
  let component: BaskanMesaji;
  let fixture: ComponentFixture<BaskanMesaji>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaskanMesaji]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BaskanMesaji);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
