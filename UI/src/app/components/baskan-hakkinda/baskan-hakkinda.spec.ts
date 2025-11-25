import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaskanHakkinda } from './baskan-hakkinda';

describe('BaskanHakkinda', () => {
  let component: BaskanHakkinda;
  let fixture: ComponentFixture<BaskanHakkinda>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BaskanHakkinda]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BaskanHakkinda);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
