import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RedesSociaisComponent } from './redes-sociais.component';

describe('RedesSociaisComponent', () => {
  let component: RedesSociaisComponent;
  let fixture: ComponentFixture<RedesSociaisComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RedesSociaisComponent]
    });
    fixture = TestBed.createComponent(RedesSociaisComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
