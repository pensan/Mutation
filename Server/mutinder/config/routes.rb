Rails.application.routes.draw do
  # For details on the DSL available within this file, see http://guides.rubyonrails.org/routing.html

  # API routing ###############################################################
  # Defines a scoped 'namespace'.
  # @param [String] module  The name of the module
  # @param [String] path    The path where the module gets mounted to
  # @param [String] as      The name of the url path helper (e.g. 'api_example_path')
  #
  scope module: 'api', path: '/api', as: 'api', defaults: {format: :json} do

    resources :users
  end

  root to: "base#serverstatus"
end
