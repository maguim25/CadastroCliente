import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RelacionamentoClienteComponent } from './relacionamento-cliente.component';

describe('RelacionamentoClienteComponent', () => {
  let component: RelacionamentoClienteComponent;
  let fixture: ComponentFixture<RelacionamentoClienteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RelacionamentoClienteComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RelacionamentoClienteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
