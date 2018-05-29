Custom Entry
=============

Custom Entry con bindable properties para indicar:

 - El color del borde (por ejemplo para indicar errores usando un Behavior).

 - El tipo del botón 'Return' del teclado.

 - Una referencia al siguiente Entry al que llevar el foco con el botón 'Return' del teclado, cuando este sea de tipo 'Next'.

 - Una referencia a un Button para ejecutar su Command desde el botón 'Return' del teclado, cuando este sea de tipo 'Done'.


## Ejemplo de uso

```

<controls:CustomEntry x:Name="entryUsername"
                    ReturnKeyType="Next"
                    NextEntry="{Binding Source={x:Reference entryEmail}}">
</controls:CustomEntry>

<controls:CustomEntry x:Name="entryEmail"
                    ReturnKeyType="Next"
                    NextEntry="{Binding Source={x:Reference entryPassword}}">
</controls:CustomEntry>

<!-- Otros Entry... -->

<controls:CustomEntry x:Name="entryPassword"
                    IsPassword="True"
                    ReturnKeyType="Done"
                    DoneButton="{Binding Source={x:Reference buttonCreateAccount}}">
</controls:CustomEntry>

<Button x:Name="buttonCreateAccount"
        Command="{Binding CreateAccountCommand}"/>
  
```
