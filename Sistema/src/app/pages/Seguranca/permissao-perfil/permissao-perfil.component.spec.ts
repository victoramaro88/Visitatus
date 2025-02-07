import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissaoPerfilComponent } from './permissao-perfil.component';

describe('PermissaoPerfilComponent', () => {
  let component: PermissaoPerfilComponent;
  let fixture: ComponentFixture<PermissaoPerfilComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PermissaoPerfilComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PermissaoPerfilComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
