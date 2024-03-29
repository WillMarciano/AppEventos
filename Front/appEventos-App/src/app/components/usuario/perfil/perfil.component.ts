import { Component, OnInit } from '@angular/core';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { environment } from '@environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss'],
})
export class PerfilComponent implements OnInit {
  public usuario = {} as UserUpdate;
  public imagemUrl = '';
  public file: File;
  
  public get ehPalestrante(): boolean {
    return this.usuario.funcao === 'Palestrante';
  }

  constructor(private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private accountService: AccountService) {}

  ngOnInit(): void {}

  public setFormValue(usuario: UserUpdate): void {
    this.usuario = usuario;
    if (this.usuario.imagemUrl)
      this.imagemUrl = environment.apiURL + 'resources/perfil/' + this.usuario.imagemUrl;
      
    else
      this.imagemUrl = './assets/img/perfil.png';

  }

  public onFileChange(ev: any): void {
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemUrl = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImagem();
  }

  uploadImagem(): void {
    this.spinner.show();
    this.accountService
      .postUpload(this.file)
      .subscribe({
        next: () => {
          this.toastr.success('Imagem atualizada com sucesso', 'Sucesso');
        },
        error: (error: any) => {
          this.toastr.error('Erro ao fazer Upload de imagem', 'Erro');
          console.log(error);
        },
      })
      .add(() => this.spinner.hide());
  }
}

